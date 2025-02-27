import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";

const AppointmentSuccessfullyCreated = () => {
  const navigate = useNavigate();
  const handleBackHome = () => {
    navigate("/");
  };

  return (
    <div>
      <div>Your appointment has been successfully created!</div>
      <Button onClick={handleBackHome}>Head back home</Button>
    </div>
  );
};

export default AppointmentSuccessfullyCreated;
