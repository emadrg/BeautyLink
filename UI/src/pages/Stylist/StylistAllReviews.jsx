import { Box, Rating } from "@mui/material";

const DisplayAllReviews = ({ text, score }) => {
  return (
    <div>
      <p>
        <Box
          component="fieldset"
          borderColor="transparent"
          style={{ height: 35 }}
        >
          <Rating name="read-only" value={score} readOnly />
        </Box>
        <i> "{text}"</i>
      </p>
    </div>
  );
};

export default DisplayAllReviews;
