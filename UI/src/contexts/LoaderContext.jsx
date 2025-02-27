import { createContext, useContext, useState } from "react";

const LoaderContext = createContext({
  loading: false,
  setLoading: () => {},
});

export const LoadingProvider = (props) => {
  const [loading, setLoading] = useState(false);

  return (
    <LoaderContext.Provider value={{ loading, setLoading }}>
      {props.children}
    </LoaderContext.Provider>
  );
};

export const useLoader = () => useContext(LoaderContext);
