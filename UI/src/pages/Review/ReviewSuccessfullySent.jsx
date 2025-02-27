import { Button } from "@mui/material";
import { useNavigate } from "react-router-dom";
const ReviewSuccessfullySent = () => {
  const navigate = useNavigate();

  const handleBackHome = () => {
    navigate("/");
  };
  return (
    <div>
      <div>Review has been successfully sent</div>
      <Button onClick={handleBackHome}>Head back home</Button>
    </div>
  );
};

export default ReviewSuccessfullySent;
