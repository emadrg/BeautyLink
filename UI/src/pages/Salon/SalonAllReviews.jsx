import { Box, Rating } from "@mui/material";

const SalonAllReviews = ({ id, salonId, text, score }) => {
  return (
    <div className="single-review">
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

export default SalonAllReviews;
