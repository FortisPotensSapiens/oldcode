import 'yup-phone-lite';

import { yupResolver } from '@hookform/resolvers/yup';
import { LoadingButton } from '@mui/lab';
import { Alert, AlertTitle, Box, Grid } from '@mui/material';
import { useSnackbar } from 'notistack';
import { useEffect, useMemo } from 'react';
import { FormProvider, useForm, useFormState } from 'react-hook-form';
import { useParams } from 'react-router-dom';
import { useDebounce } from 'usehooks-ts';
import * as yup from 'yup';

import { useGetPartnerById } from '~/api';
import { useGetIndividualOrder } from '~/api/v1/useGetIndividualOrder';
import { useGetIndividualOrderOfferInfo } from '~/api/v1/useGetIndividualOrderOfferInfo';
import { usePostDeliveryIndividualOrder } from '~/api/v1/usePostDeliveryIndividualOrder';
import { PageLayout } from '~/components';
import { AddressReadModel, OrderCreateModel } from '~/types';
import { ORDER_VARIANT_SHIPPING, OrderForm, OrderVariant } from '~/views/NewOrderView/OrderForm';

import IndividualOrderDetail from './IndividualOrderDetail/IndividualOrderDetail';
import { usePlaceAndPay } from './usePlaceAndPay';

export type FormData = Pick<AddressReadModel, 'apartmentNumber' | 'entrance' | 'house' | 'intercom' | 'street'> &
  Pick<OrderCreateModel, 'comments' | 'recipientFullName' | 'recipientPhone'> & {
    variant: OrderVariant;
  };

const validationSchema = yup.object({
  recipientPhone: yup.string().phone().required(),
});

const PlaceIndividualOrder = () => {
  const { individualOrderOfferId, individualOrderId } = useParams();
  const { enqueueSnackbar } = useSnackbar();

  const defaultValues: FormData = {
    apartmentNumber: '',
    comments: '',
    entrance: '',
    house: '',
    intercom: '',
    recipientFullName: '',
    recipientPhone: '',
    street: '',
    variant: ORDER_VARIANT_SHIPPING,
  };

  const { handleSubmit, watch, ...methods } = useForm({
    defaultValues,
    mode: 'all',
    resolver: yupResolver(validationSchema),
  });
  const { isValid } = useFormState({ control: methods.control });
  const { data: offerData } = useGetIndividualOrderOfferInfo(String(individualOrderOfferId), {
    enabled: !!individualOrderOfferId,
  });
  const { data: sellerData } = useGetPartnerById(String(offerData?.seller.id), {
    enabled: !!offerData?.seller.id,
  });
  const { data: individualOrderData } = useGetIndividualOrder(String(individualOrderId));

  const variant = watch('variant');
  const debouncedStreet = useDebounce(watch('street'), 1000);
  const debouncedHouse = useDebounce(watch('house'), 1000);

  const [send, { error, isError: isFuckup, isLoading: isSending }] = usePlaceAndPay(String(individualOrderOfferId));

  const deliveryCalculate = usePostDeliveryIndividualOrder({
    offerId: String(individualOrderOfferId),
    recipientAddress: {
      street: debouncedStreet,
      house: debouncedHouse,
      latitude: 0,
      longitude: 0,
      zipCode: '0',
    },
  });

  const isDeliveryCalculatingError = useMemo(() => {
    return deliveryCalculate.isError && variant === ORDER_VARIANT_SHIPPING;
  }, [deliveryCalculate.isError, variant]);

  useEffect(() => {
    methods.trigger();
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    if (error) {
      // eslint-disable-next-line
      enqueueSnackbar(Object.values((error as any)?.response?.data?.errors).join('\n'), {
        variant: 'error',
      });
    }
  }, [enqueueSnackbar, error]);

  return (
    <PageLayout maxWidth={1156} title="Оформление индивидуального заказа">
      <FormProvider handleSubmit={handleSubmit} watch={watch} {...methods}>
        <Box component="form" mx={{ sm: 0, xs: 2 }} onSubmit={handleSubmit(send)}>
          <Grid alignItems="flex-start" columnSpacing={3} container mt={3}>
            <Grid container item md={6} xs={12}>
              <OrderForm seller={sellerData} variant={defaultValues.variant} />
            </Grid>

            <Grid container item md={6} xs={12}>
              <Box width={1}>
                {offerData && individualOrderData ? (
                  <IndividualOrderDetail
                    variant={variant}
                    deliveryCost={deliveryCalculate.data?.price}
                    deliveryCostFetching={deliveryCalculate.isFetching}
                    deliveryCostFetched={deliveryCalculate.isFetched}
                    isDeliveryCalculatingError={isDeliveryCalculatingError}
                    individualOrderData={individualOrderData}
                    offerData={offerData}
                  >
                    {isFuckup && (
                      <Box component={Alert} mb={3} severity="error">
                        <AlertTitle>Ошибка</AlertTitle>
                        <p>При оформлении заказа произошла ошибка!</p>

                        <p>
                          <strong>Попробуйте еще раз!</strong>
                        </p>
                      </Box>
                    )}

                    <LoadingButton
                      loading={isSending}
                      disabled={
                        isSending ||
                        !isValid ||
                        isDeliveryCalculatingError ||
                        (variant === ORDER_VARIANT_SHIPPING && deliveryCalculate.isFetching)
                      }
                      fullWidth
                      type="submit"
                      variant="contained"
                    >
                      Оплатить
                    </LoadingButton>
                  </IndividualOrderDetail>
                ) : undefined}
              </Box>
            </Grid>
          </Grid>
        </Box>
      </FormProvider>
    </PageLayout>
  );
};

export default PlaceIndividualOrder;
