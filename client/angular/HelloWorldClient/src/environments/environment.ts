export const environment = {
  authentication: {
    client_id: 'client',
    client_secret: 'secret',
    grant_type: 'password',
  },
  apiPaths: {
    identity: 'http://localhost:5001/identity/',
    words: 'http://localhost:5001/words/',
    achievements: 'http://localhost:5001/achievements/',
    getUserById: (id: number) => {
      return `http://localhost:5001/words/api/user/${id}`;
    },
    registerUser: () => {
      return `http://localhost:5001/identity/api/users`;
    },
    getWordCollections: () => {
      return 'http://localhost:5001/words/api/wordcollection';
    },
    getWordCollectionById: (id: number) => {
      return `http://localhost:5001/words/api/wordcollection/${id}`;
    },
    addWordToDictionary: (wordId: number) => {
      return `http://localhost:5001/words/api/dictionary/words/${wordId}`;
    },
  },
};
