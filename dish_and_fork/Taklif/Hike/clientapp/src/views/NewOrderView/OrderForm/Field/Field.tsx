import { FC } from 'react';
import { useFormContext } from 'react-hook-form';

import { FormController, FormControllerProps } from './FormController';
import { Input, InputProps } from './Input';
import { useSaveChangesToStore } from './useSaveChangesToStore';

type FieldProps = Pick<FormControllerProps, 'name' | 'required'> & Pick<InputProps, 'label' | 'multiline'>;

const Field: FC<FieldProps> = ({ label, multiline, name, required }) => {
  const saveChangesToStore = useSaveChangesToStore(name);

  const {
    formState: { errors },
  } = useFormContext();

  return (
    <FormController
      name={name}
      render={({ field: { onChange, ...field } }) => (
        <Input
          error={!!errors[name]}
          helperText={errors[name]?.message}
          label={label}
          multiline={multiline}
          onChange={(event) => {
            saveChangesToStore(event);
            onChange(event);
          }}
          {...field}
        />
      )}
      rules={{ required }}
    />
  );
};

export { Field };
export type { FieldProps };
