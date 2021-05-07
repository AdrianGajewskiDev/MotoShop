//base
export const serverBaseUrl: string = "https://localhost:44304/";

//identity
export const serverRegisterNewUserUrl = "https://localhost:44304/api/identity/register";
export const serverSignInUrl = "https://localhost:44304/api/identity/signIn";
export const serverUserProfileDetailsUrl = "https://localhost:44304/api/UserAccount/details";
export const serverUpdateUserProfileUrl = "https://localhost:44304/api/UserAccount/updateUserData";
export const serverUpdateUserPasswordUrl = "https://localhost:44304/api/UserAccount/changePassword";
export const serverResetUserPasswordUrl = "https://localhost:44304/api/UserAccount/resetPassword"
export const serverExternalSignInUrl = "https://localhost:44304/api/identity/externalSignIn";
export const serverRefreshTokenUrl = "https://localhost:44304/api/identity/refreshToken";

//administration
export const administrationGetAllUsers = "https://localhost:44304/api/administration";
export const administrationAddRoleToUserUrls = "https://localhost:44304/api/administration/addRole";
export const administrationDeleteUserUrls = "https://localhost:44304/api/administration/";
export const administrationSeedDatabaseUrls = "https://localhost:44304/api/administration/seedDb";

//advertisements
export const serverGetAllAdvertisementsByUserIDUrl = "https://localhost:44304/api/advertisements/byUser";
export const serverGetAllAdvertisementsUrl = "https://localhost:44304/api/advertisements";
export const serverDeleteAdvertisementUrl = "https://localhost:44304/api/advertisements/";
export const serverUpdateAdvertisementUrl = "https://localhost:44304/api/advertisements/";
export const serverGetTopAdvertisementsUrl = "https://localhost:44304/api/advertisements/topThree";
export const serverAddAdvertisementUrl = "https://localhost:44304/api/advertisements";

//files
export const serverImageUploadUrl = "https://localhost:44304/api/upload/userProfile";
export const serverAdvertisementImageUploadUrl = "https://localhost:44304/api/upload/adImages";

//server health
export const serverOverallHealthUrl = "https://localhost:44304/api/serverHealth";

//watchlist
export const serverWatchlistUrl = "https://localhost:44304/api/watchlist";
export const serverAddToWatchlistUrl = "https://localhost:44304/api/watchlist";
export const serverDeleteToWatchlistUrl = "https://localhost:44304/api/watchlist";

//Conversations
export const serverGetConversationUrl = "https://localhost:44304/api/messages";
export const serverSendMessageUrl = "https://localhost:44304/api/messages";

