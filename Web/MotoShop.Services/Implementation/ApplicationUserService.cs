using Microsoft.AspNetCore.Identity;
using MotoShop.Data.Database_Context;
using MotoShop.Data.Helpers;
using MotoShop.Data.Models.User;
using MotoShop.Services.HelperModels;
using MotoShop.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotoShop.Services.Implementation
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDatabaseContext _dbContext;
        private UserManager<ApplicationUser> _userManager;

        public ApplicationUserService(ApplicationDatabaseContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<ApplicationUser> GetUserByID(string id) => await _userManager.FindByIdAsync(id);

        public async Task<ApplicationUser> GetUserByUserName(string username) => await _userManager.FindByNameAsync(username);
      
        public async Task<bool> RegisterNewUserAsync(ApplicationUser user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<string> SignInAsync(string data, string password, UserSignInVariant variant)
        {
            string userID = string.Empty;
            switch (variant)
            {
                case UserSignInVariant.UserName:
                    {
                        var user = await _userManager.FindByNameAsync(data);
                        if(await _userManager.CheckPasswordAsync(user, password))
                        {
                            userID = user.Id;
                        }
                    }
                    break;
                case UserSignInVariant.Email:
                    {
                        var user = await _userManager.FindByEmailAsync(data);
                        if (await _userManager.CheckPasswordAsync(user, password))
                        {
                            userID = user.Id;
                        }
                    }
                    break;
            }

            return userID;
        }

        //TODO: Add email notifications about the changes
        //  *In email editing case, we should first send confirmation email to the old email adress
        public async Task<UpdateResult> UpdateUserDataAsync(string userID, ApplicationUser model)
        {
            ApplicationUser user = await GetUserByID(userID);

            if (user == null)
                return new UpdateResult 
                {
                    Result = false
                };

            var dataToUpdate = ChechWhatDataToUpdate(model);

            if(dataToUpdate.Count == 0)
                return new UpdateResult
                {
                    Result = false
                };

            foreach (var data in dataToUpdate)
            {
                switch (data)
                {
                    case UpdateDataType.Username:
                        {
                            if (UserExists(model) != 2) 
                            {
                                user.UserName = model.UserName;
                                await _userManager.UpdateNormalizedUserNameAsync(user);
                            }
                            else
                                return new UpdateResult
                                {
                                    ErrorIdentificator = 2,
                                    Result = false
                                };
                                
                        }
                        break;
                    case UpdateDataType.Email:
                        {
                            if (UserExists(model) != 1)
                                user.Email = model.Email;
                            else
                                return new UpdateResult
                                {
                                    ErrorIdentificator = 1,
                                    Result = false
                                };
                        }

                        break;
                    case UpdateDataType.PhoneNumber:
                        user.PhoneNumber = model.PhoneNumber;
                        break;
                    case UpdateDataType.Name:
                        user.Name = model.Name;
                        break;
                    case UpdateDataType.Lastname:
                        user.LastName = model.LastName;
                        break;
                }
            }

            var result = await _dbContext.SaveChangesAsync();
            if (result < 0)
                return new UpdateResult
                {
                    Result = false
                };


            return new UpdateResult
            {
                Result = true
            };
        }

        /// <summary>
        /// Returns if UserName or Email is already taken
        /// </summary>
        /// <param name="user">\The user</param>
        /// <returns>0 if email and username are not taken</returns>
        /// <returns>1 if email is taken</returns>
        /// <returns>2 if username is taken</returns>
        public int UserExists(ApplicationUser user)
        {
            int result = 0;

            if (_dbContext.Users.Where(x => x.Email == user.Email).Count() > 0)
                result = 1;

            if (_dbContext.Users.Where(x => x.UserName == user.UserName).Count() > 0)
                result = 2;

            return result;
        }

        private List<UpdateDataType> ChechWhatDataToUpdate(ApplicationUser user)
        {
            List<UpdateDataType> dataTypes = new List<UpdateDataType>();

            if (!string.IsNullOrEmpty(user.Email))
                dataTypes.Add(UpdateDataType.Email);
            if (!string.IsNullOrEmpty(user.UserName))
                dataTypes.Add(UpdateDataType.Username);
            if (!string.IsNullOrEmpty(user.Name))
                dataTypes.Add(UpdateDataType.Name);
            if (!string.IsNullOrEmpty(user.LastName))
                dataTypes.Add(UpdateDataType.Lastname);
            if (!string.IsNullOrEmpty(user.PhoneNumber))
                dataTypes.Add(UpdateDataType.PhoneNumber);

            return dataTypes;
            
        }
    }
}
