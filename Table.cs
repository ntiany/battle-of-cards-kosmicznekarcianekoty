using System;
using System.Collections.Generic;

namespace Card_Game
{
    public class Table
    {
        private List<Player> players;
        private TableDeck tableDeck;
        private TableDeck roundDeck;
        private TableDeck benchDeck;
        private Player roundWinner;

        public Table(params string[] playersNames) // first inserted player will start the game
        {
            CreateTableDeck();
            CreateRoundDeck();
            CreateBenchDeck();
            CreatePlayers(playersNames);
        }

        private void CreateTableDeck()
        {
            ICardsDAO fullDeck = new CardsDAO();
            tableDeck = new TableDeck(fullDeck);
        }

        private void CreateRoundDeck()
        {
            roundDeck = new TableDeck();
        }

        private void CreateBenchDeck()
        {
            benchDeck = new TableDeck();
        }

        private void CreatePlayers(string[] playersNames)
        {
            players = new List<Player>();
            foreach (string name in playersNames)
            {
                Player newPlayer = new Player();
                newPlayer.Name = name; // let's add 'name' parameter to Player's consturctor?
                players.Add(newPlayer);
            }
        }
        
        public void DealCards()
        {
            tableDeck.Shuffle();
            SetCardsToPlay();
            while (!tableDeck.IsEmpty())
            {
                 foreach (Player player in players)
                 {
                    List<Card> oneCardToDeal = tableDeck.GetTopCards(1);
                    player.PlayerDeck.AddCardToDeckBottom(oneCardToDeal[0]);
                    oneCardToDeal[0].ChangeOwner(player);
                    tableDeck.RemoveTopCards(1);
                 }
            }
        }

        private void SetCardsToPlay()
        {
            int cardsToRemove = tableDeck.Cards.Count % players.Count;
            tableDeck.RemoveTopCards(cardsToRemove);
        }

        public void EndOfRound(CardsAttributes chosenAttribute)
        {
            if (roundDeck.IsTie(chosenAttribute))
            {
                CopyCardsToDeckFromDeck(benchDeck, roundDeck);
            }
            else
            {
                Card highestCard = roundDeck.GetHighestCard(chosenAttribute);
                roundWinner = highestCard.Owner;
                CopyCardsToWinnerDeck();
                SetStartingPlayer();
            }
            roundDeck.Cards.Clear();
        }
        
        private void PlayersPlayCard()
        {
            foreach (Player player in players)
            {
                List<Card> oneCardToPlay = player.PlayerDeck.GetTopCards(1);
                player.PlayerDeck.RemoveTopCard();
                roundDeck.AddCardsToDeckBottom(oneCardToPlay);
            }
        }

        private void CopyCardsToDeckFromDeck(TableDeck toDeck, TableDeck fromDeck)
        {
            toDeck.AddCardsToDeckBottom(fromDeck.Cards);
        }

        private void CopyCardsToWinnerDeck()
        {
            CopyCardsToDeckFromDeck(roundDeck, benchDeck);
            roundDeck.ChangeCardsOwner(roundWinner);
            CopyCardsToDeckFromDeck(roundWinner.PlayerDeck, roundDeck);
        }

        private void SetStartingPlayer()
        {
            if (roundWinner != players[0])
            {
                int winnerIndex = players.IndexOf(roundWinner);
                players.RemoveAt(winnerIndex);
                players.Insert(0, roundWinner);
            }
        }

        public bool IsGameOver()
        {
            foreach (Player player in players)
            {
                if (player.PlayerDeck.IsEmpty())
                {
                    return true;
                }
            }
            return false;
        }

        public Player GetWinner()
        {
            Player winner = players[0];
            foreach (Player player in players)
            {
                if (player.PlayerDeck.Cards.Count > winner.PlayerDeck.Cards.Count)
                {
                    winner = player;
                }
            }
            return winner;
        }

    }
}
