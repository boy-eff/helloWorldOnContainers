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
    getWordCollections: () => {
      return 'http://localhost:5001/words/api/wordcollection';
    },
  },
};
