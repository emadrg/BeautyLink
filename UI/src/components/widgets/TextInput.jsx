import { Alert, FormControl, TextField } from "@mui/material";

const TextInput = (props) => {

    const {error} = props;

  return (
    <FormControl>
      <TextField
        {...props}
      />
      {error && (
        <Alert severity="error">{error}</Alert>
      )}
    </FormControl>
  );
};

export default TextInput;