using System;
using Manatee.Trello;
using System.Threading.Tasks;

namespace LSPLibrary
{
    public interface Plataforma
    {
        void CardCreator();
        void CreateCard(string text);
        IList GetEntryPointList(IBoard board);
        Task<ICard> CreateCard(IList list, String message);
        Task<IListCollection> GetTasks(IBoard board);
        IBoard GetMainBoard();
        Task<IBoardCollection> GetBoards();
    }
    public class TrelloCardCreator : Plataforma
    {
        private ITrelloFactory factory;

        public void CardCreator()
        {
            TrelloAuthorization.Default.AppKey = "2b35f8f94fbae185bb2148f6fa962e43";
            TrelloAuthorization.Default.UserToken = "2eb429d0560f3f93ffd3e92eba415b6e68e13f6cfb6006d12ded2f7deef0e23a";

        }

        public void CreateCard(string text)
        {
            IBoard board = new Board("AUHiDaH1");
            IList firstList = this.GetEntryPointList(board);

            Task<ICard> createCard = this.CreateCard(firstList, text);
            createCard.Wait();
        }

        public async Task<ICard> CreateCard(IList list, String message)
        {
            ICard card = await list.Cards.Add(message);
            return card;
        }

        public IList GetEntryPointList(IBoard board)
        {
            Task<IListCollection> task = this.GetTasks(board);
            task.Wait();

            IListCollection lists = task.Result;
            return lists[0];
        }

        public async Task<IListCollection> GetTasks(IBoard board)
        {
            await board.Refresh();
            return board.Lists;
        }

        public IBoard GetMainBoard()
        {
            Task<IBoardCollection> getBoards = this.GetBoards();
            getBoards.Wait();

            IBoardCollection boards = getBoards.Result;
            return boards[0];
        }

        public async Task<IBoardCollection> GetBoards()
        {
            var me = await factory.Me();
            return me.Boards;
        }
    }
}