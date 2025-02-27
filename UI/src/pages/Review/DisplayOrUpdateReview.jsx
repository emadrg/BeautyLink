import { useState } from "react";
import LeaveReview from "./LeaveReview";
import UpdateReview from "./UpdateReview";

const DisplayOrUpdateReview = ({
  existingReviewObject,
  updateReviewMethod,
  addReviewMethod,
  receiverType,
}) => {
  const [addReviewButtonIsPressed, setAddReviewButtonIsPressed] =
    useState(false);

  return (
    <div>
      {existingReviewObject && (
        <UpdateReview
          existingReviewObject={existingReviewObject}
          updateReviewMethod={updateReviewMethod}
          receiverType={`${receiverType}`}
        ></UpdateReview>
      )}
      {!existingReviewObject && (
        <button
          onClick={() => setAddReviewButtonIsPressed(!addReviewButtonIsPressed)}
        >
          Leave a review
        </button>
      )}
      {addReviewButtonIsPressed && (
        <LeaveReview
          addReviewMethod={addReviewMethod}
          receiverType={`${receiverType}`}
        ></LeaveReview>
      )}
    </div>
  );
};

export default DisplayOrUpdateReview;
