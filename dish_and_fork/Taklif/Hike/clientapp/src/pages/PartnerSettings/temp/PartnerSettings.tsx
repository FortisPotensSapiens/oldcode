import { Box, Button, Checkbox, FormControlLabel, Grid, Paper, Stack, TextField } from '@mui/material';
import { FC } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { Navigate } from 'react-router-dom';

import { BasicHeader, LoadingSpinner, SmBackButton } from '~/components';
import { useCanBeSeller } from '~/hooks';
import { getPartnerSettingsPath, getStorefrontPath } from '~/routing';
import { PartnerType, PartnerUpdateModel } from '~/types';

import { getStored, STORAGE_KEY } from './getStored';
import { PhotoField } from './PhotoField';
import { Type } from './Type';
import { useChecked } from './useChecked';

const PartnerSettings: FC = () => {
  const [checked, changeChecked] = useChecked(true);
  const canBeSeller = useCanBeSeller();

  const {
    control,
    formState: { errors },
    handleSubmit,
    watch,
  } = useForm<PartnerUpdateModel>({
    // defaultValues: {
    //   contactEmail: oidcUser.profile.email ?? '',
    //   contactPhone: '',
    //   inn: '',
    //   title: '',
    //   type: PartnerType.SelfEmployed,
    //   ...getStored(),
    // },
  });

  watch((data) => {
    window.localStorage.setItem(STORAGE_KEY, JSON.stringify(data));
  });

  const submit = (data: unknown) => console.log(data);

  if (canBeSeller === null) {
    return <LoadingSpinner />;
  }

  if (!canBeSeller) {
    return <Navigate replace to={getPartnerSettingsPath()} />;
  }

  return (
    <Box
      borderRadius={({ shape }) => ({ sm: shape.bigBorderRadius, xs: 0 })}
      component={Paper}
      height={{ sm: 'auto', xs: '100%' }}
      marginX="auto"
      marginY={{ lg: 5, md: 4, sm: 3, xs: 0 }}
      maxWidth={920}
      paddingX={{ sm: 3, xs: 2 }}
      pb={{ sm: 3, xs: 2 }}
    >
      <Box component="form" onSubmit={handleSubmit(submit)}>
        <SmBackButton href={getStorefrontPath()} />

        <BasicHeader justifyContent="center" storefront textAlign="center">
          Стать продавцом
        </BasicHeader>

        <Box textAlign="center">
          Рассмотрение вашей заявки займет некоторе время. После подтверждения вам нужно будет перезайти на сайт с
          новыми правами и вы сможете размещать свои товары
        </Box>

        <Box mt={{ sm: 3, xs: 2 }} textAlign="center">
          <Controller
            control={control}
            name="imageId"
            render={({ field }) => <PhotoField {...field} />}
            rules={{ required: true }}
          />
        </Box>

        <Box mb={3} mt={{ md: 8, xs: 4.25 }}>
          <Box columnSpacing={3} component={Grid} container rowSpacing={{ md: 4, xs: 3 }}>
            {/* <Grid item sm={6} xs={12}>
              <Controller
                control={control}
                name="title"
                render={({ field }) => (
                  <TextField fullWidth error={!!errors.title} label="ФИО или название компании" {...field} />
                )}
                rules={{ required: true }}
              />
            </Grid>

            <Grid item sm={6} xs={12}>
              <Controller
                control={control}
                name="inn"
                render={({ field }) => <TextField fullWidth error={!!errors.inn} label="ИНН" {...field} />}
                rules={{ required: true }}
              />
            </Grid>

            <Grid item sm={6} xs={12}>
              <Controller
                control={control}
                name="contactPhone"
                render={({ field }) => <TextField fullWidth error={!!errors.contactPhone} label="Телефон" {...field} />}
              />
            </Grid>

            <Grid item sm={6} xs={12}>
              <Controller
                control={control}
                name="contactEmail"
                render={({ field }) => (
                  <TextField fullWidth error={!!errors.contactEmail} label="Электронная почта" {...field} />
                )}
                rules={{ pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/ }}
              />
            </Grid> */}

            <Grid item sm={6} xs={12}>
              <Controller
                control={control}
                name="description"
                render={({ field }) => (
                  <TextField error={!!errors.description} fullWidth label="Описание" multiline {...field} />
                )}
              />
            </Grid>
          </Box>
        </Box>

        <Stack alignItems={{ sm: 'flex-start', xs: 'stretch' }} spacing={2}>
          {/* <Controller control={control} name="type" render={({ field }) => <Type {...field} />} /> */}

          <FormControlLabel
            control={<Box checked={checked} component={Checkbox} ml={-1.25} onChange={changeChecked} />}
            label="Я согласен на  обработку и хранение моих персональных данных"
          />

          <Button disabled={!checked} size="large" type="submit" variant="contained">
            Отправить заявку
          </Button>
        </Stack>
      </Box>
    </Box>
  );
};

export { PartnerSettings };
