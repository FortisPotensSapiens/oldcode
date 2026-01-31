import { FormControl, InputLabel, Select, SelectProps, TextField, TextFieldProps } from '@mui/material';
import { get } from 'lodash-es';
import { FC } from 'react';
import { Controller, ControllerProps, useFormContext } from 'react-hook-form';

import css from './FormField.module.scss';

type FormFieldProps = Pick<ControllerProps, 'rules' | 'name'> &
  Pick<
    TextFieldProps,
    'disabled' | 'label' | 'multiline' | 'maxRows' | 'select' | 'inputRef' | 'type' | 'placeholder' | 'InputProps'
  > &
  Pick<SelectProps, 'multiple'> & { readOnly?: boolean };

const FormField: FC<FormFieldProps> = ({ disabled = false, name, readOnly, rules, ...props }) => {
  const { children, multiple, select } = props;

  const {
    control,
    formState: { errors },
  } = useFormContext();

  const errorTexts = get(errors, name);

  const common = {
    disabled,
    error: !!get(errors, name),
    fullWidth: true,
    helperText: Array.isArray(errorTexts)
      ? errorTexts?.map((e, index) => <p key={`${e.message}_${index.toString()}`}>{e.message}</p>)
      : get(errors, name)?.message,
  };

  return (
    <Controller
      control={control}
      name={name}
      render={({ field }) => {
        field.value = field.value ?? '';

        return select && multiple ? (
          <FormControl {...common}>
            <InputLabel>{props.label}</InputLabel>
            <Select {...field} {...common} {...props} required={!!rules?.required}>
              {children}
            </Select>
          </FormControl>
        ) : (
          <TextField
            InputProps={{ classes: css, readOnly }}
            {...common}
            {...props}
            {...field}
            required={!!rules?.required}
          />
        );
      }}
      rules={rules}
    />
  );
};

export { FormField };
export type { FormFieldProps };
