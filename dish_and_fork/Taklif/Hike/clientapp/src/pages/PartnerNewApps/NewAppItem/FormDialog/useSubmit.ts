import dayjs from 'dayjs';
import timezone from 'dayjs/plugin/timezone';
import utc from 'dayjs/plugin/utc';
import { useSnackbar } from 'notistack';
import { useCallback } from 'react';

import { usePostIndividualApplicationOffers } from '~/api';
import { FileReadModel, MerchandiseUnitType, OfferCreateModel } from '~/types';
import { getNormalizedSaveUnit } from '~/utils';

import { SubmitCallback } from './types';

type FormData = Omit<OfferCreateModel, 'applicationId' | 'images' | 'date'> & {
  images: FileReadModel[];
  date: string;
};

type UseSubmit = (
  applicationId: string,
  onComplete?: SubmitCallback,
) => [
  (payload: FormData) => void,
  {
    isError: boolean;
    isLoading: boolean;
    isSuccess: boolean;
  },
];

const useSubmit: UseSubmit = (applicationId, cb) => {
  const { isError, isLoading, isSuccess, mutateAsync } = usePostIndividualApplicationOffers();
  const { enqueueSnackbar } = useSnackbar();

  const handler = useCallback(
    (payload: FormData) => {
      mutateAsync({
        ...payload,
        applicationId,
        servingGrossWeightInKilograms: getNormalizedSaveUnit(
          payload.servingGrossWeightInKilograms,
          MerchandiseUnitType.Kilograms,
        ),
        images: payload.images
          .map((image) => {
            return image.id;
          })
          .filter((n) => n),
        date: dayjs(payload.date).utc().toDate().toISOString(),
      }).then((data) => {
        cb?.(data);

        enqueueSnackbar('Отклик успешно размещен', {
          autoHideDuration: 2500,
          variant: 'success',
        });
      });
    },
    [applicationId, mutateAsync, cb, enqueueSnackbar],
  );

  return [handler, { isError, isLoading, isSuccess }];
};

export { useSubmit };
export type { FormData, SubmitCallback };
