import { TextField, TextFieldProps } from '@mui/material';
import parsePhoneNumber from 'libphonenumber-js';
import { get } from 'lodash-es';
import { FC, SyntheticEvent } from 'react';
import { Controller, ControllerProps, useFormContext } from 'react-hook-form';
import ReactPhoneInput from 'react-phone-input-material-ui';

import 'react-phone-input-material-ui/lib/style.css';
import './FormField.scss';

type FormFieldPhoneProps = Pick<ControllerProps, 'rules' | 'name'> & Pick<TextFieldProps, 'disabled' | 'label'>;

const FormFieldPhone: FC<FormFieldPhoneProps> = ({ disabled = false, name, rules, ...props }) => {
  const {
    control,
    formState: { errors },
  } = useFormContext();

  const common = { disabled, error: !!get(errors, name), fullWidth: true, rules };

  return (
    <Controller
      control={control}
      name={name}
      render={({ field }) => {
        const withPreffix = field.value.indexOf('+') > -1 ? field.value : `+${field.value}`;
        field.value = withPreffix;

        return (
          <ReactPhoneInput
            {...common}
            {...props}
            {...field}
            autoFormat
            component={TextField}
            country="ru"
            inputClass="phone-input"
            inputProps={{
              required: rules?.required,
              ...common,

              onChange: (value: SyntheticEvent<HTMLInputElement>) => {
                const phoneNumber = parsePhoneNumber(value.currentTarget.value);

                field.onChange(phoneNumber?.number ?? value);
              },
            }}
            jumpCursorToEnd
          />
        );
      }}
      rules={rules}
    />
  );
};

export { FormFieldPhone };
export type { FormFieldPhoneProps };
