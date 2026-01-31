import { yupResolver } from '@hookform/resolvers/yup';
import { LoadingButton } from '@mui/lab';
import { Box, Grid } from '@mui/material';
import { styled } from '@mui/system';
import Modal from 'antd/es/modal/Modal';
import { useSnackbar } from 'notistack';
import { FC, useCallback, useEffect, useState } from 'react';
import { FormProvider, useForm, useFormState } from 'react-hook-form';
import * as yup from 'yup';

import { useGetFileById, usePutPartner } from '~/api';
import { UploadImage } from '~/components';
import SMSConfirmContainer from '~/components/SMSConfirm/SMSConfirmContainer';
import { PartnerReadModel, PartnerUpdateModel } from '~/types';

import { PartnerSettingsTabs } from './PartnerSettingsTabs';
import { FormData } from './types';

const StyledBox = styled(Box)(({ theme }) => ({
  [theme.breakpoints.down('md')]: {
    margin: theme.spacing(1),
    width: '100%',
  },
  backgroundColor: theme.palette.background.paper,
  margin: theme.spacing(2),
  padding: theme.spacing(3),
  width: '730px',
}));

const validationSchema = yup.object({
  address: yup.object().shape({
    house: yup.string().required(),
    street: yup.string().required(),
  }),
  registrationAddress: yup.object().shape({
    house: yup.string().required(),
    street: yup.string().required(),
  }),
  contactEmail: yup.string().email().required(),
  inn: yup.string().required(),
  title: yup.string().required(),
  workingDays: yup.array().min(1),
});

type PartnerFormProps = { data: PartnerReadModel; onUpdated: () => void };

const PartnerForm: FC<PartnerFormProps> = ({ data, onUpdated }) => {
  const getDefaultValues = useCallback((data) => {
    return {
      address: data.address ?? {},
      registrationAddress: data.registrationAddress ?? {},
      contactEmail: data.contactEmail ?? '',
      contactPhone: data.contactPhone ?? '',
      description: data.description ?? '',
      id: data.id,
      imageId: data.image?.id ?? null,
      inn: data.inn ?? '',
      title: data.title ?? '',
      workingDays: data.workingDays ?? [],
      workingTime: data.workingTime ?? {},
      isPickupEnabled: data.isPickupEnabled,
    };
  }, []);

  const { enqueueSnackbar } = useSnackbar();
  const [file, setFile] = useState('');
  const { data: uploadedFileData } = useGetFileById(file);
  const methods = useForm<FormData>({
    defaultValues: getDefaultValues(data),
    mode: 'all',
    resolver: yupResolver(validationSchema),
  });
  const [isModalOpen, setModalOpen] = useState(false);
  const contactPhone = methods.watch('contactPhone');
  const [submitValues, setSubmitValues] = useState<FormData>({} as any);

  const onSmsConfirm = () => {
    setModalOpen(true);
  };

  const onModalClose = () => {
    setModalOpen(false);
  };

  const { isDirty, isValid } = useFormState({
    control: methods.control,
  });

  const putPartner = usePutPartner();

  const submitHandler = (event: FormData) => {
    if (event.contactPhone === data.contactPhone) {
      const update: PartnerUpdateModel = {
        ...event,
        id: data.id,
      };
      putPartner.mutate(update);
    } else {
      setSubmitValues(event);
      onSmsConfirm();
    }
  };

  const onCodeSuccessCallback = (code: string) => {
    const update: PartnerUpdateModel = {
      ...submitValues,
      id: data.id,
      phoneComfinmationCode: code,
      isPickupEnabled: true,
    };

    putPartner.mutate(update);
    onModalClose();
  };

  useEffect(() => {
    if (putPartner.isError) {
      enqueueSnackbar('Ошибка обновления профиля', {
        variant: 'error',
      });
    }
  }, [putPartner.isError, enqueueSnackbar]);

  useEffect(() => {
    if (putPartner.isSuccess) {
      enqueueSnackbar('Профиль обновлен', {
        variant: 'success',
      });

      onUpdated();
    }
  }, [putPartner.isSuccess, enqueueSnackbar, onUpdated]);

  // сразу делаем валидацию.
  useEffect(() => {
    methods.trigger();
  }, [methods]);

  const onUploadComplete = (fileGuid: string[]) => {
    setFile(fileGuid[0]);
  };

  useEffect(() => {
    methods.setValue('imageId', file, { shouldDirty: true });
  }, [file]);

  useEffect(() => {
    methods.reset(getDefaultValues(data));
  }, [data, methods, getDefaultValues]);

  return (
    <FormProvider {...methods}>
      <Modal open={isModalOpen} footer={false} onCancel={onModalClose}>
        <SMSConfirmContainer phoneNumber={contactPhone} onSuccess={onCodeSuccessCallback} />
      </Modal>

      <StyledBox component="form" onSubmit={methods.handleSubmit(submitHandler)}>
        <Grid alignItems="center" container display="flex" flexDirection="column" spacing={4}>
          <Grid item>
            <UploadImage
              accept="image/*"
              borderRadius="50%"
              height="116px"
              onComplete={onUploadComplete}
              photo={uploadedFileData?.path ?? data.image?.path ?? undefined}
              width="116px"
            />
          </Grid>

          <Grid item width="100%">
            <PartnerSettingsTabs>
              <LoadingButton
                fullWidth
                disabled={!isDirty || !isValid}
                loading={putPartner.isLoading}
                onClick={methods.handleSubmit(submitHandler)}
                variant="contained"
              >
                Сохранить
              </LoadingButton>
            </PartnerSettingsTabs>
          </Grid>
        </Grid>
      </StyledBox>
    </FormProvider>
  );
};

export { PartnerForm };
export type { PartnerFormProps };
