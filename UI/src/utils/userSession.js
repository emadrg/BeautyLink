var sessionExpirationTimeoutId = null;
var sessionExpirationTime = 60 * 60 * 1000; // expiration time set to 1 hour

export default {
  saveAuthSession: (jwt, userDetails = null, onExpirationCallback = null) => {
    localStorage.setItem("jwt", jwt);
    if (userDetails) {
      localStorage.setItem("userDetails", JSON.stringify(userDetails));
    }
    localStorage.setItem("expirationTime", Date.now() + sessionExpirationTime);
    if (onExpirationCallback) {
      clearTimeout(sessionExpirationTimeoutId);
      sessionExpirationTimeoutId = setTimeout(
        onExpirationCallback,
        sessionExpirationTime
      );
    }
  },
  isAuthenticated: () => {
    return localStorage.getItem("jwt") != null && !isAuthSessionExpired();
  },
  clearAuthSession: () => {
    localStorage.removeItem("jwt");
    localStorage.removeItem("userDetails");
    localStorage.removeItem("expirationTime");
    clearTimeout(sessionExpirationTimeoutId);
  },
  user: () => {
    return JSON.parse(localStorage.getItem("userDetails"));
  },
  token: () => {
    return localStorage.getItem("jwt");
  },
};

let isAuthSessionExpired = () => {
  var expirationTime = localStorage.getItem("expirationTime");
  return expirationTime <= Date.now();
};
