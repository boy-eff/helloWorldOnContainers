export const environment = {
    baseUrl: "http://localhost:5001/",
    authentication: {
        client_id: "client",
        client_secret: "secret",
        grant_type: "password"
    },
    apiPaths: {
        identity: "http://localhost:5001/identity/",
        words: () => environment.baseUrl + "words/",
        achievements: () => environment.baseUrl + "achievements/"
    }
}