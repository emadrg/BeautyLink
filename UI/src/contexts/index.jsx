import React from "react";
import { LoadingProvider } from "./LoaderContext";
import { AccountProvider } from "./AccountContext";

const AppProvider = ({ children }) => {
  return (
    <>
      <LoadingProvider>
        <AccountProvider>{children}</AccountProvider>
      </LoadingProvider>
    </>
  );
};

export default AppProvider;
