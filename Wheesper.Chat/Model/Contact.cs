
namespace Wheesper.Chat.Model
{
    public class Contact
    {
        private string email = null;
        private string nickName = null;
        private string sex = null;
        private int age = 0;
        private string country = null;
        private string province = null;
        private string city = null;
        private string create_date = null;

        public string EMail { get; set; }
        public string NickName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Create_date { get; set; }

        public Contact()
        {

        }
    }
}
