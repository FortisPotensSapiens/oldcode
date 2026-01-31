// eslint-disable-next-line simple-import-sort/imports
import { yupResolver } from '@hookform/resolvers/yup';
import { LoadingButton } from '@mui/lab';
import { Box, Checkbox, FormControlLabel, Grid, Paper, Stack } from '@mui/material';
import { FC, useEffect, useState } from 'react';
import { Controller, FormProvider, useForm, useFormState } from 'react-hook-form';
import * as yup from 'yup';

import { Alert, Button, Modal, notification, Tooltip, Typography } from 'antd';
import { useOidc } from '@axa-fr/react-oidc';
import styled from '@emotion/styled';
import { BasicHeader, FormField, SmBackButton } from '~/components';
import { FormFieldPhone } from '~/components/FormFieldPhone/FormFieldPhone';
import { getStorefrontPath } from '~/routing';
import { PartnerCreateModel } from '~/types';

import { STORAGE_KEY } from './getStored';
import { Type } from './Type';
import { useChecked } from './useChecked';
import { useSubmit } from './useSubmit';

import 'yup-phone-lite';
import SMSConfirmContainer from '~/components/SMSConfirm/SMSConfirmContainer';
import { useConfig } from '~/contexts';

const StyledSubTitle = styled(Typography.Title)`
  font-weight: normal !important;
`;

type ApplicationFormProps = {
  defaultValues: Partial<PartnerCreateModel>;
  completed?: boolean;
};

const validationSchema = yup.object({
  contactEmail: yup.string().email(),
  contactPhone: yup.string().phone().required(),
  title: yup.string().required(),
  registrationAddress: yup.object().shape({
    house: yup.string().required(),
    street: yup.string().required(),
  }),
});

const ApplicationForm: FC<ApplicationFormProps> = ({ completed = false, defaultValues }) => {
  const [checked, changeChecked] = useChecked(true);
  const { renewTokens } = useOidc();
  const config = useConfig();
  const [submit, { isError, isLoading, isSuccess }] = useSubmit(checked);
  const isCompleted = isSuccess || completed;
  const disabled = isCompleted || isLoading;
  const [isModalOpen, setModalOpen] = useState(false);
  const [api, contextHolder] = notification.useNotification();

  const methods = useForm<PartnerCreateModel>({ defaultValues, mode: 'all', resolver: yupResolver(validationSchema) });
  const { control, handleSubmit, watch } = methods;

  const { isDirty, isValid } = useFormState({ control });
  const contactPhone = methods.watch('contactPhone');

  useEffect(() => {
    if (isSuccess) {
      renewTokens().then(() => {
        api.success({
          message: 'Поздравляем, Вы стали продавцом',
          description:
            'Ваш магазин находится на модерации. Вы можете размещать товары, но доступны пользователям будут после модерации.',
          placement: 'topRight',
        });
      });

      setTimeout(() => {
        window.location.href = '/';
      }, 1500);
    }
  }, [api, isCompleted]);

  const onSmsConfirm = () => {
    setModalOpen(true);
  };

  const onModalClose = () => {
    setModalOpen(false);
  };

  watch((data) => {
    window.localStorage.setItem(STORAGE_KEY, JSON.stringify(data));
  });

  const onCodeSuccessCallback = (code: string) => {
    onModalClose();
    submit({
      ...methods.getValues(),
      phoneComfinmationCode: code,
    });
  };

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
      {contextHolder}
      <FormProvider {...methods}>
        <Box component="form" onSubmit={handleSubmit(onSmsConfirm)}>
          <SmBackButton href={getStorefrontPath()} />

          <BasicHeader justifyContent="center" storefront textAlign="center">
            Стать продавцом
          </BasicHeader>

          <Modal open={isModalOpen} footer={false} onCancel={onModalClose}>
            <SMSConfirmContainer phoneNumber={contactPhone} onSuccess={onCodeSuccessCallback} />
          </Modal>

          <Grid container spacing={3}>
            <Grid item sm={6} xs={12}>
              <FormField
                disabled={disabled}
                label="ФИО или название компании"
                name="title"
                rules={{ required: true }}
              />
            </Grid>

            <Grid item sm={6} xs={12}>
              <FormField disabled={disabled} label="ИНН/ОГРНИП/ОГРН" name="inn" rules={{ required: true }} />
            </Grid>

            <Grid item sm={6} xs={12}>
              <FormField
                disabled={disabled}
                label="Электронная почта"
                name="contactEmail"
                rules={{ pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/, required: true }}
              />
            </Grid>

            <Grid item sm={6} xs={12}>
              <FormFieldPhone disabled={disabled} label="Телефон" name="contactPhone" rules={{ required: true }} />
            </Grid>
          </Grid>

          <StyledSubTitle level={4}>Юридический адрес</StyledSubTitle>

          <Grid container spacing={3} marginBottom={6}>
            <Grid item sm={6} xs={12}>
              <FormField
                label="Улица"
                disabled={disabled}
                maxRows={5}
                name="registrationAddress.street"
                rules={{
                  required: true,
                }}
              />
            </Grid>

            <Grid item sm={6} xs={12}>
              <FormField
                disabled={disabled}
                label="Дом"
                maxRows={5}
                name="registrationAddress.house"
                rules={{
                  required: true,
                }}
              />
            </Grid>

            <Grid item sm={6} xs={12}>
              <FormField label="Квартира" disabled={disabled} maxRows={5} name="registrationAddress.apartmentNumber" />
            </Grid>

            <Grid item sm={6} xs={12}>
              <FormField label="Домофон" disabled={disabled} name="registrationAddress.intercom" />
            </Grid>
          </Grid>

          <Grid container spacing={3}>
            <Grid item sm={6} xs={12}>
              <Tooltip title="Без регистрации в платежной системе магазин будет находиться в демо режиме и проведение платежей будет не возможно">
                <Button type="default" target="_blank" href={config.yooReferalLink}>
                  Зарегистироваться в платежной системе.
                </Button>
              </Tooltip>
            </Grid>
          </Grid>

          <Box
            alignItems={{ sm: 'flex-start', xs: 'stretch' }}
            component={Stack}
            mt={{ sm: 1 }}
            spacing={{ sm: 2, xs: 3 }}
          >
            <Controller control={control} name="type" render={({ field }) => <Type disabled={disabled} {...field} />} />

            <FormControlLabel
              control={
                <Box checked={checked} component={Checkbox} disabled={disabled} ml={-1.25} onChange={changeChecked} />
              }
              label="Я согласен на  обработку и хранение моих персональных данных"
            />

            {isError && (
              <Alert type="error" message="Во время отправки запроса произошла ошибка. Попробуйте еще раз." />
            )}

            {isCompleted ? undefined : (
              <LoadingButton
                disabled={!checked || !isDirty || !isValid}
                loading={isLoading}
                size="large"
                type="submit"
                variant="contained"
              >
                Отправить заявку
              </LoadingButton>
            )}
          </Box>
        </Box>
      </FormProvider>
    </Box>
  );
};

export { ApplicationForm };
export type { ApplicationFormProps };
