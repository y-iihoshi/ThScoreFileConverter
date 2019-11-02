using ThScoreFileConverter.Models.Th105;

namespace ThScoreFileConverterTests.Models.Th105.Stubs
{
    internal class CardForDeckStub : ICardForDeck
    {
        public CardForDeckStub() { }

        public CardForDeckStub(ICardForDeck cardForDeck)
            : base()
        {
            this.Id = cardForDeck.Id;
            this.MaxNumber = cardForDeck.MaxNumber;
        }

        public int Id { get; set; }

        public int MaxNumber { get; set; }
    }
}
