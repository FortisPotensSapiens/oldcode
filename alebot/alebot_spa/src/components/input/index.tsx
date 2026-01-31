import React, { FC } from "react";

import { TextField, TextFieldProps } from "@mui/material";

import styles from "./styles.module.scss";

const Input: FC<TextFieldProps> = (props) => {
  return (
    <div className={styles.formGroup}>
      <TextField {...props} />
    </div>
  );
};

export default Input;
