import { Grid, MenuItem, Paper } from '@mui/material';
import { Checkbox, Typography } from 'antd';
import { FC } from 'react';
import { Controller, useFormContext } from 'react-hook-form';

import { FormField, TimePickerFormField } from '~/components';
import { FormFieldPhone } from '~/components/FormFieldPhone/AntFormFieldPhone';

const PartnerSettingsTabs: FC = ({ children }) => {
  const { control } = useFormContext();

  return (
    <Paper>
      <Grid container spacing={3} marginBottom={6}>
        <Grid item sm={6} xs={12}>
          <FormField
            label="ФИО или название компании"
            name="title"
            rules={{
              required: true,
            }}
          />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField
            label="Адрес электронной почты"
            name="contactEmail"
            rules={{
              required: true,
            }}
          />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField label="Описание" maxRows={5} multiline name="description" />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormFieldPhone label="Телефон" name="contactPhone" rules={{ required: true }} />
        </Grid>
      </Grid>

      <Grid container spacing={3} marginBottom={6}>
        <Grid item xs={12}>
          <FormField
            label="ИНН/ОГРНИП/ОГРН"
            maxRows={5}
            name="inn"
            rules={{
              required: true,
            }}
          />
        </Grid>
      </Grid>

      <Typography.Title level={4}>Фактический адрес</Typography.Title>

      <Grid container spacing={3} marginBottom={6}>
        <Grid item sm={6} xs={12}>
          <FormField
            label="Улица"
            maxRows={5}
            name="address.street"
            rules={{
              required: true,
            }}
          />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField
            label="Дом"
            maxRows={5}
            name="address.house"
            rules={{
              required: true,
            }}
          />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField label="Квартира" maxRows={5} name="address.apartmentNumber" />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField label="Домофон" name="address.intercom" />
        </Grid>
      </Grid>

      <Typography.Title level={4}>Юридический адрес</Typography.Title>

      <Grid container spacing={3} marginBottom={6}>
        <Grid item sm={6} xs={12}>
          <FormField
            label="Улица"
            maxRows={5}
            name="registrationAddress.street"
            rules={{
              required: true,
            }}
          />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField
            label="Дом"
            maxRows={5}
            name="registrationAddress.house"
            rules={{
              required: true,
            }}
          />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField label="Квартира" maxRows={5} name="registrationAddress.apartmentNumber" />
        </Grid>

        <Grid item sm={6} xs={12}>
          <FormField label="Домофон" name="registrationAddress.intercom" />
        </Grid>
      </Grid>

      <Grid container spacing={3} marginBottom={6}>
        <Grid item xs={12}>
          <FormField label="Выберите день недели" multiple name="workingDays" select>
            <MenuItem value="Monday">Понедельник</MenuItem>
            <MenuItem value="Tuesday">Вторник</MenuItem>
            <MenuItem value="Wednesday">Среда</MenuItem>
            <MenuItem value="Thursday">Четверг</MenuItem>
            <MenuItem value="Friday">Пятница</MenuItem>
            <MenuItem value="Saturday">Суббота</MenuItem>
            <MenuItem value="Sunday">Воскресенье</MenuItem>
          </FormField>
        </Grid>

        <Grid item sm={6} xs={12}>
          <TimePickerFormField label="Время начала работ" name="workingTime.start" />
        </Grid>

        <Grid item sm={6} xs={12}>
          <TimePickerFormField label="Время конца работ" name="workingTime.end" />
        </Grid>

        <Grid item sm={6} xs={12}>
          <Controller
            control={control}
            name="isPickupEnabled"
            render={({ field }) => {
              return (
                <Checkbox {...field} checked={field.value}>
                  Самовывоз по адресу доступен
                </Checkbox>
              );
            }}
          />
        </Grid>
      </Grid>

      {children}
    </Paper>
  );
};

export { PartnerSettingsTabs };
