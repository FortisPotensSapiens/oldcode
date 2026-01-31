import { yupResolver } from '@hookform/resolvers/yup/dist/yup';
import { Close } from '@mui/icons-material';
import { LoadingButton } from '@mui/lab';
import { Box, Dialog, DialogActions, Grid, IconButton, styled, Typography } from '@mui/material';
import dayjs from 'dayjs';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';
import { useSnackbar } from 'notistack';
import { useEffect } from 'react';
import { FormProvider, useForm, useFormState } from 'react-hook-form';
import * as yup from 'yup';

import { usePostNewIndividualOrder } from '~/api/v1/usePostNewIndividualOrder';
import { FormField, FormFieldDatePicker } from '~/components';
import { useDownSm } from '~/hooks';
import { ApplicationCreateModel } from '~/types/swagger';
import { getCurrencySymbol } from '~/utils';

dayjs.extend(utc);
dayjs.extend(timezone);

const StyledDialogActions = styled(DialogActions)({
  justifyContent: 'left',
  padding: 0,
});

const StyledDialogContainer = styled(Box)(({ theme }) => ({
  padding: theme.spacing(2),
}));

const StyledDialogContent = styled(Box)(({ theme }) => ({
  paddingBottom: theme.spacing(2),
  paddingTop: theme.spacing(2),
}));

const validationSchema = yup.object({
  fromDate: yup.date().required().min(dayjs().startOf('day').toDate(), 'Нельзя указать раньше сегодняшнего дня'),
  toDate: yup
    .date()
    .required()
    .when('fromDate', (fromDate, schema) => {
      return schema.min(fromDate ?? new Date(), 'Финальная дата должна быть больше минимальной');
    })
    .min(dayjs().startOf('day').toDate(), 'Нельзя указать раньше сегодняшнего дня'),
  sumFrom: yup.number().required().min(1),
  sumTo: yup
    .number()
    .required()
    .min(1)
    .moreThan(yup.ref('sumFrom'), 'Максимальная сумма должна быть больше минимальной'),
  title: yup.string().required(),
});

const CreateIndividualOrderDialog = ({ handleClose, isOpen }: { isOpen: boolean; handleClose: () => void }) => {
  const isSmall = useDownSm();
  const { enqueueSnackbar } = useSnackbar();
  const methods = useForm<ApplicationCreateModel>({
    mode: 'all',
    resolver: yupResolver(validationSchema),
  });

  const { isDirty, isValid } = useFormState({
    control: methods.control,
  });

  const submitForm = usePostNewIndividualOrder();

  const onSubmit = ({ fromDate: dateFrom, toDate: dateTo, ...data }: ApplicationCreateModel) => {
    const fromDate = dateFrom && dayjs(dateFrom).utc().toISOString();
    const toDate = dateTo && dayjs(dateTo).utc().toISOString();
    submitForm.mutate({ ...data, fromDate, toDate });
  };

  useEffect(() => {
    if (submitForm.isSuccess) {
      methods.reset();
      handleClose();

      enqueueSnackbar('Создан индивидуальный заказ', {
        variant: 'success',
      });
    }
  }, [enqueueSnackbar, handleClose, methods, submitForm.isSuccess]);

  return (
    <FormProvider {...methods}>
      <Dialog onClose={handleClose} open={isOpen}>
        <StyledDialogContainer>
          <Box component={IconButton} mr={2} mt={2} onClick={handleClose} position="absolute" right={0} top={0}>
            <Close />
          </Box>
          <Typography marginRight={6} variant="h5">
            Создать заявку на индивидуальный заказ
          </Typography>
          <StyledDialogContent>
            <Grid columnSpacing={3} container rowSpacing={3}>
              <Grid item xs={12}>
                <FormField label="Название" name="title" rules={{ required: true }} />
              </Grid>
              <Grid item sm={6} xs={12}>
                <FormField
                  label="Цена, от"
                  InputProps={{ endAdornment: getCurrencySymbol('Rub') }}
                  name="sumFrom"
                  rules={{ required: true }}
                  type="number"
                />
              </Grid>
              <Grid item sm={6} xs={12}>
                <FormField
                  label="Цена, до"
                  InputProps={{ endAdornment: getCurrencySymbol('Rub') }}
                  name="sumTo"
                  rules={{ required: true }}
                  type="number"
                />
              </Grid>
              <Grid item sm={6} xs={12}>
                <FormFieldDatePicker label="Дата,с" name="fromDate" rules={{ required: true }} />
              </Grid>
              <Grid item sm={6} xs={12}>
                <FormFieldDatePicker label="Дата, до" name="toDate" rules={{ required: true }} />
              </Grid>
              <Grid item xs={12}>
                <FormField label="Описание" name="description" />
              </Grid>
            </Grid>
          </StyledDialogContent>
          <StyledDialogActions>
            <LoadingButton
              disabled={!isDirty || !isValid}
              fullWidth={isSmall}
              loading={submitForm.isLoading}
              onClick={methods.handleSubmit(onSubmit)}
              variant="contained"
            >
              Создать заявку
            </LoadingButton>
          </StyledDialogActions>
        </StyledDialogContainer>
      </Dialog>
    </FormProvider>
  );
};

export default CreateIndividualOrderDialog;
