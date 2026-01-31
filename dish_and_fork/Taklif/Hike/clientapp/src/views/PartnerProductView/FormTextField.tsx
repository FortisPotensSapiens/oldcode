import { TextField, TextFieldProps } from '@mui/material';
import { FC } from 'react';
import { Controller, ControllerProps, useFormContext } from 'react-hook-form';

import { MerchandiseUpdateModel } from '~/types';

type FormTextFieldProps = Pick<ControllerProps, 'rules'> &
  TextFieldProps & {
    linked?: keyof MerchandiseUpdateModel;
    name: keyof MerchandiseUpdateModel;
    valueAsNumber?: boolean;
    valueAsFloat?: boolean;
  };

const FormTextField: FC<FormTextFieldProps> = ({
  linked,
  name,
  onBlur,
  rules,
  valueAsNumber = false,
  valueAsFloat = false,
  ...props
}) => {
  const {
    control,
    formState: { errors },
  } = useFormContext<MerchandiseUpdateModel>();

  const error = errors[name];
  const linkedError = linked && errors[linked];
  const text = Array.isArray(error) ? error[0]?.message : error?.message;

  return (
    <Controller
      control={control}
      name={name}
      render={({ field: { onBlur: onFieldBlur, ...field } }) => {
        const mutateNumber = (value: string | number | undefined | null | string[]) => {
          if (valueAsNumber) {
            value = parseInt(String(value), 10) ?? 0;

            if (isNaN(value)) {
              value = '';
            }
          }

          if (valueAsFloat) {
            value = String(value).replace(/[^\d.-]/g, '');
          }

          return value;
        };

        const value = mutateNumber(field.value);

        return (
          <TextField
            error={Boolean(errors[name] || linkedError)}
            fullWidth
            helperText={text}
            onBlur={(event) => {
              onFieldBlur();

              if (onBlur) {
                onBlur(event);
              }
            }}
            {...props}
            {...field}
            onChange={(event) => {
              field.onChange(mutateNumber(event.target.value as string | number));
            }}
            value={value}
          />
        );
      }}
      rules={rules}
    />
  );
};

export { FormTextField };
export type { FormTextFieldProps };
