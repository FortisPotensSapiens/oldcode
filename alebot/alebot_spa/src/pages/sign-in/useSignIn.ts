import { emailValidator } from "@/helpers/emailValidator";
import { ISignInErrors, ISignInValues } from "./singIn.interface";
import { REQUIRED_TEXT, WRONG_EMAIL_FORMAT } from "@/constants/messages";
import { useRouter } from "next/router";
import { signInRequest } from "@/service/auth";
import * as amplitude from '@amplitude/analytics-browser';

const useSignIn = () => {
  const initialValues: ISignInValues = { email: "", password: "" };
  const router = useRouter();

  const onSubmit = async (values: ISignInValues) => {
    const response = await signInRequest(values);

    if (response?.accessToken && response?.refreshToken) {
      amplitude.setUserId(values.email);
      amplitude.track('Sign In');
      localStorage.setItem("accessToken", response.accessToken);
      localStorage.setItem("refreshToken", response.refreshToken);
      router.push("/products");
    }
  };

  const valitdate = (values: ISignInValues) => {
    const errors: ISignInErrors = {};
    if (!values.email) {
      errors.email = REQUIRED_TEXT;
    } else if (emailValidator(values.email)) {
      errors.email = WRONG_EMAIL_FORMAT;
    }
    if (!values.password) {
      errors.password = REQUIRED_TEXT;
    }
    return errors;
  };
  
  return { initialValues, onSubmit, valitdate };
};

export default useSignIn;
