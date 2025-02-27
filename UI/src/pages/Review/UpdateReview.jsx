import StarIcon from "@mui/icons-material/Star";
import { Box, Modal, Rating, Stack, TextField } from "@mui/material";
import { useState } from "react";
import { useParams } from "react-router-dom";

const UpdateReview = ({
  existingReviewObject,
  updateReviewMethod,
  receiverType,
}) => {
  const [hover, setHover] = useState(-1);
  const [updatedReview, setUpdatedReview] = useState({
    ...existingReviewObject,
  });
  const [isEditable, setIsEditable] = useState(false);
  const [modalIsOpen, setModalIsOpen] = useState(false);

  const { id: receiverId } = useParams();

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

  const handleModalState = () => {
    setModalIsOpen(!modalIsOpen);
  };

  const handleCancel = () => {
    setUpdatedReview({ ...existingReviewObject });
    setIsEditable(false);
  };

  const handleUpdateReview = async () => {
    let reviewObject = {
      text: updatedReview.text,
      score: updatedReview.score,
    };
    reviewObject[`${receiverType}`] = receiverId;
    try {
      await updateReviewMethod(reviewObject);
      handleModalState();
    } catch (err) {
      console.log(err);
    }
  };
  return (
    <div className="leave-review-container">
      <div className="leave-review-container-contents">
        <Stack gap={2}>
          <Rating
            name="hover-feedback"
            value={updatedReview.score}
            precision={1}
            disabled={!isEditable}
            getLabelText={getLabelText}
            onChange={(event, newValue) => {
              setUpdatedReview({
                ...updatedReview,
                score: newValue,
              });
            }}
            onChangeActive={(event, newHover) => {
              setHover(newHover);
            }}
            emptyIcon={
              <StarIcon style={{ opacity: 0.55 }} fontSize="inherit" />
            }
          />
          {updatedReview.score && (
            <Box sx={{ ml: 2 }}>
              {labels[hover !== -1 ? hover : updatedReview.score]}
            </Box>
          )}

          <TextField
            className="review-text-field"
            label="Review"
            multiline
            value={updatedReview.text}
            onChange={(event) => {
              setUpdatedReview({ ...updatedReview, text: event.target.value });
            }}
            maxRows={4}
            disabled={!isEditable}
          />
        </Stack>
        {!isEditable && (
          <button onClick={() => setIsEditable(true)}>Update Review</button>
        )}
        {isEditable && (
          <>
            <button onClick={handleUpdateReview}>Save</button>
            <button onClick={handleCancel}>Cancel</button>
          </>
        )}

        <div>
          <Modal open={modalIsOpen} onClose={() => handleModalState()}>
            <Box sx={{ ...style }}>
              <div>Review successfully sent!</div>
              <button
                style={{ marginLeft: 90, marginTop: 20 }}
                onClick={() => handleModalState()}
              >
                ok
              </button>
            </Box>
          </Modal>
        </div>
      </div>
    </div>
  );
};

export default UpdateReview;
