const authentication = {
  clientId: 'client',
  clientSecret: 'secret',
  grantType: 'password',
  refreshGrantType: 'refresh_token',
};

const apiPaths = {
  tokenEndpoint: 'http://localhost:5001/identity/connect/token',
  wordCollectionTestEndpoint: (id: number) =>
    `http://localhost:5001/words/collectionhub/${id}`,
  achievementsEndpoint: `http://localhost:5001/achievements/api/achievements`,
  usersAchievementsEndpoint: (id: number) =>
    `http://localhost:5001/achievements/api/users/${id}/achievements`,
  wordsUserEndpoint: (id: number) =>
    `http://localhost:5001/words/api/user/${id}`,
  identityUsersEndpoint: 'http://localhost:5001/identity/api/users',
  wordCollectionEndpoint: 'http://localhost:5001/words/api/wordcollection',
  wordCollectionWithIdEndpoint: (id: number) =>
    `http://localhost:5001/words/api/wordcollection/${id}`,
  addWordToDictionaryEndpoint: (wordId: number) =>
    `http://localhost:5001/words/api/dictionary/words/${wordId}`,
  changePasswordEndpoint: 'http://localhost:5001/identity/api/users/password',
  updateUserImageEndpoint: 'http://localhost:5001/words/api/user/image',
};

export const environment = {
  authentication,
  apiPaths,
};
