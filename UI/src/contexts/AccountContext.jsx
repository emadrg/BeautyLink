import React, { createContext, useContext, useState } from "react";

const AccountContext = createContext();

const AccountProvider = ({ children }) => {
  const [isAuth, setIsAuth] = useState(false);

  const store = {
    isAuth,
    setIsAuth,
  };

  return (
    <AccountContext.Provider value={store}>{children}</AccountContext.Provider>
  );
};

const useAccount = () => useContext(AccountContext);

export { AccountProvider, AccountContext, useAccount };
