import {
  FormControl,
  InputLabel,
  Select as MaterialSelect,
  MenuItem,
} from "@mui/material";

export const Select = (props) => {
  const { options, label } = props;
  return (
    <FormControl>
      <InputLabel>{label}</InputLabel>
      <MaterialSelect {...props}>
        {options.map((o) => (
          <MenuItem key={o.id} value={o.id}>
            {o.name}
          </MenuItem>
        ))}
      </MaterialSelect>
    </FormControl>
  );
};
