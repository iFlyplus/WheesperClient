
namespace Wheesper.Chat.Model
{
    public class Contact
    {
        private string email = null;
        private string nickname = null;
        private string group = null;
        private string remarks = null;
        private string sex = null;
        private int age = 0;

        public string EMail
        {
            get { return email; }
            set { email = value; }
        }
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }
        public string Group
        {
            get { return group; }
            set { group = value; }
        }
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
    }
}
