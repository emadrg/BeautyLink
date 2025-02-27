import React from "react";
import { useLocation } from "react-router-dom";

const ErrorPage = () => {
  const location = useLocation();
  const { status, message } = location.state || {
    status: "Unknown",
    message: "An error occurred",
  };

  return (
    <div>
      <h1>Error {status}</h1>
      <p>{message}</p>
    </div>
  );
};

export default ErrorPage;
