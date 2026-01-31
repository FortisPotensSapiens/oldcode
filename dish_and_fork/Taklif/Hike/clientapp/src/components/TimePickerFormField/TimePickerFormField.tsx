import { TimePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';
import utc from 'dayjs/plugin/utc';
import { FC } from 'react';
import { useFormContext } from 'react-hook-form';

import { FormField, FormFieldProps } from '~/components';

dayjs.extend(utc);

type TimePickerFormFieldProps = FormFieldProps;

const TimePickerFormField: FC<FormFieldProps> = ({ disabled = false, name, readOnly, rules, ...props }) => {
  const { setValue, watch } = useFormContext();
  const value = watch(name);
  const onChange = (value: string | null) => {
    setValue(name, value, { shouldDirty: true });
  };

  return (
    <TimePicker
      onChange={onChange}
      renderInput={(params) => <FormField {...params} name={params.name ?? ''} />}
      value={value}
    />
  );
};

export { TimePickerFormField };
export type { TimePickerFormFieldProps };
