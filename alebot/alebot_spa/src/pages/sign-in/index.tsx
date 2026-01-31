import React, { useEffect } from "react";
import { Button, Typography } from "@mui/material";
import Input from "@/components/input";
import Link from "next/link";
import { Formik } from "formik";

import useSignIn from "./useSignIn";
import styles from "./styles.module.scss";
import { useRouter } from "next/router";

const SignInPage = () => {
  const { initialValues, onSubmit, valitdate } = useSignIn();
  const router = useRouter();
  useEffect(() => {
    const token = localStorage.getItem("accessToken");
    if(token){
      router.push("/")
    }
  }, []);
  return (
    <Formik
      initialValues={initialValues}
      onSubmit={onSubmit}
      validate={valitdate}
    >
      {({
        values,
        errors,
        touched,
        handleChange,
        handleBlur,
        handleSubmit,
        submitForm,
      }) => {
        return (
          <form className={styles.formContent} onSubmit={handleSubmit}>
            <Input
              name="email"
              label="Электронная почта"
              variant="filled"
              onChange={handleChange}
              onBlur={handleBlur}
              error={!!errors.email}
              value={values.email}
            />
            <Input
              name="password"
              type="password"
              label="Пароль"
              variant="filled"
              onChange={handleChange}
              onBlur={handleBlur}
              error={!!errors.password}
              value={values.password}
            />
            <Link href="/forgot">
              <Typography className={styles.forgotLink}>
                Забыли пароль?
              </Typography>
            </Link>
            {(errors.email && touched.email && (
              <span className="errorMessage">{errors.email}</span>
            )) ||
              (errors.password && touched.password && (
                <span className="errorMessage">{errors.password}</span>
              ))}
            <Button variant="contained" onClick={submitForm}>
              Далее
            </Button>
          </form>
        );
      }}
    </Formik>
  );
};

export default SignInPage;
