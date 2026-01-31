import { TextField, TextFieldProps } from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';
import { get } from 'lodash-es';
import { FC } from 'react';
import { Controller, ControllerProps, useFormContext } from 'react-hook-form';

type FormFieldPhoneProps = Pick<ControllerProps, 'rules' | 'name'> & Pick<TextFieldProps, 'disabled' | 'label'>;

const FormFieldDatePicker: FC<FormFieldPhoneProps> = ({ disabled = false, name, rules, ...props }) => {
  const {
    control,
    formState: { errors },
  } = useFormContext();

  const common = { disabled, error: !!get(errors, name), fullWidth: true };
  const errorMessage = get(errors, name);

  return (
    <Controller
      control={control}
      name={name}
      render={({ field }) => {
        field.value = field.value ?? '';

        return (
          <DatePicker
            inputFormat="DD.MM.YYYY"
            {...field}
            {...common}
            renderInput={(params) => (
              <TextField
                {...params}
                {...common}
                {...props}
                {...field}
                required={!!rules?.required}
                helperText={errorMessage?.message}
              />
            )}
          />
        );
      }}
      rules={rules}
    />
  );
};

export { FormFieldDatePicker };
