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
  achievementsEndpoint: (id: number) =>
    `http://localhost:5001/achievements/api/users/${id}/achievements`,
  getUserById: (id: number) => `http://localhost:5001/words/api/user/${id}`,
  registerUser: 'http://localhost:5001/identity/api/users',
  wordCollectionEndpoint: 'http://localhost:5001/words/api/wordcollection',
  getWordCollectionById: (id: number) =>
    `http://localhost:5001/words/api/wordcollection/${id}`,
  addWordToDictionary: (wordId: number) =>
    `http://localhost:5001/words/api/dictionary/words/${wordId}`,
};

export const environment = {
  authentication,
  apiPaths,
};
