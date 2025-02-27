import StarIcon from "@mui/icons-material/Star";
import { Box, Modal, Rating, TextField } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

const LeaveReview = ({ addReviewMethod, receiverType }) => {
  const [inputValue, setInputValue] = useState("");
  const [reviewIsSent, setReviewIsSent] = useState(false);
  const [value, setValue] = useState(3);
  const [hover, setHover] = useState(-1);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [buttonIsPressed, setButtonIsPressed] = useState(false);

  const { id: receiverId } = useParams();

  const navigate = useNavigate();

  const labels = {
    1: "Bad",
    2: "Poor",
    3: "Good",
    4: "Very Good",
    5: "Excellent",
  };

  const style = {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    width: 350,
    height: 150,
    bgcolor: "background.paper",
    border: "2px solid #000",
    boxShadow: 24,
    p: 4,
    paddingLeft: 5,
  };

  const getLabelText = (value) => {
    return `${value} Star${value !== 1 ? "s" : ""}, ${labels[value]}`;
  };

  const handleAddReview = async () => {
    let reviewObject = {
      text: inputValue,
      score: value,
    };
    reviewObject[`${receiverType}`] = receiverId;
    try {
      await addReviewMethod(reviewObject);
      setReviewIsSent(true);
      handleModalState();
    } catch (err) {
      console.log(err);
    }
  };

  const handleModalState = () => {
    setModalIsOpen(!modalIsOpen);
  };

  useEffect(() => {}, []);

  const handlePageReload = () => {
    setButtonIsPressed(true);
  };

  return (
    <div>
      {!reviewIsSent && (
        <div>
          <Rating
            name="hover-feedback"
            value={value}
            precision={1}
            getLabelText={getLabelText}
            onChange={(event, newValue) => {
              setValue(newValue);
            }}
            onChangeActive={(event, newHover) => {
              setHover(newHover);
            }}
            emptyIcon={
              <StarIcon style={{ opacity: 0.55 }} fontSize="inherit" />
            }
          />
          {value !== null && (
            <Box sx={{ ml: 2 }}>{labels[hover !== -1 ? hover : value]}</Box>
          )}

          <TextField
            id="outlined-multiline-flexible"
            label="Review"
            multiline
            value={inputValue}
            onChange={(event) => {
              setInputValue(event.target.value);
            }}
            maxRows={4}
          />

          <button onClick={handleAddReview}>Add review</button>

          <div>
            <Modal open={modalIsOpen} onClose={() => handleModalState()}>
              <Box sx={{ ...style }}>
                <div>Review successfully sent!</div>
                <button
                  style={{ marginLeft: 90, marginTop: 20 }}
                  onClick={() => handlePageReload()}
                >
                  ok
                </button>
              </Box>
            </Modal>
          </div>
        </div>
      )}
    </div>
  );
};

export default LeaveReview;
