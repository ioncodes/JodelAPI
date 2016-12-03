using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JodelAPI.Shared;

namespace JodelAPI
{
    public class Jodel
    {
        #region Fields and Properties

        public User Account { get; private set; }

        #endregion

        #region Constructor

        public Jodel(User user)
        {
            this.Account = user;
        }

        public Jodel(string place, string countrCode, string cityName)
            : this(new User
            {
                CountryCode = countrCode,
                CityName = cityName,
                Place = new Location(place)
            })
        { }

        #endregion

        #region Methods

        public bool GenerateAccessToken()
        {
            return Account.Token.GenerateNewAccessToken();
        }

        public bool RefrashAccessToken()
        {
            return Account.Token.RefreshAccessToken();
        }

        #endregion
    }
}
