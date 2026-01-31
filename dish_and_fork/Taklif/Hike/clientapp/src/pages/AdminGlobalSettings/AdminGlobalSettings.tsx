import styled from '@emotion/styled';
import { yupResolver } from '@hookform/resolvers/yup';
import { Button, Form, notification, Skeleton, Tabs, TabsProps } from 'antd';
import { useCallback, useEffect, useMemo } from 'react';
import { FormProvider, useForm } from 'react-hook-form';
import * as yup from 'yup';

import { useGetAdminGlobalSettings } from '~/api/v1/useGetAdminGlobalSettings';
import { usePutAdminGlobalSettings } from '~/api/v1/usePutAdminGlobalSettings';
import { PageLayout } from '~/components/PageLayout';
import { useDownSm } from '~/hooks';
import { GlobaSettingsModel } from '~/types';

import { CommisionForDelivery } from './CommisionForDelivery/CommisionForDelivery';
import { CommisionForOrder } from './CommisionForOrder/CommisionForOrder';

const StyledContainer = styled.div`
  padding-left: 1rem;
  padding-right: 1rem;
`;

const validationSchema = yup.object({
  minMerchPrice: yup.number().required(),
  merchCoefficient: yup.number().required(),
  minDeliveryPrice: yup.number().required(),
  deliveryCoefficient: yup.number().required(),
});

const AdminGlobalSettings = () => {
  const form = useForm<GlobaSettingsModel>({
    mode: 'all',
    resolver: yupResolver(validationSchema),
  });
  const mutation = usePutAdminGlobalSettings();
  const [api, contextHolder] = notification.useNotification();
  const { isLoading, data, refetch } = useGetAdminGlobalSettings();
  const isSmall = useDownSm();
  const onSubmit = useCallback(
    (updated: Partial<GlobaSettingsModel>) => {
      const newData: GlobaSettingsModel = {
        ...data,
        ...updated,
        minMerchPrice: 0,
      } as GlobaSettingsModel;

      mutation.mutate(newData);
    },
    [data, mutation],
  );
  const getSubmitButton = useCallback(() => {
    return (
      <Button
        disabled={!form.formState.isDirty || !form.formState.isValid}
        loading={mutation.isLoading}
        size="large"
        onClick={form.handleSubmit(onSubmit)}
      >
        Сохранить
      </Button>
    );
  }, [form, mutation.isLoading, onSubmit]);
  const tabWrapper = useCallback(
    (componentRender: () => void) => {
      if (isLoading || !data) {
        return <Skeleton />;
      }

      return (
        <>
          {componentRender()}
          {getSubmitButton()}
        </>
      );
    },
    [data, getSubmitButton, isLoading],
  );
  const items: TabsProps['items'] = useMemo(() => {
    return [
      {
        key: '1',
        label: 'Комиссия за доставку',
        children: tabWrapper(() => <CommisionForDelivery />),
      },
      {
        key: '2',
        label: `Комиссия за товар`,
        children: tabWrapper(() => <CommisionForOrder />),
      },
    ];
  }, [tabWrapper]);

  useEffect(() => {
    if (mutation.isSuccess) {
      refetch();

      api.success({
        message: 'Настройки обновлены',
        duration: 2,
      });
    }
  }, [api, mutation.isSuccess, refetch]);

  useEffect(() => {
    if (data) {
      form.reset(data);
    }
  }, [data, form]);

  return (
    <PageLayout title="Глобальные настройки">
      {contextHolder}
      <StyledContainer>
        <FormProvider {...form}>
          <Form name="basic" layout="vertical">
            <Tabs items={items} tabPosition={isSmall ? 'top' : 'left'} />
          </Form>
        </FormProvider>
      </StyledContainer>
    </PageLayout>
  );
};

export { AdminGlobalSettings };
