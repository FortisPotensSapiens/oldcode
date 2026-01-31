import { useMutation, UseMutationResult } from 'react-query';

import { RatingUpdateModel } from '~/types';
import { extractData } from '~/utils';

import { useAxios } from './axios';

type UsePutUpgradeRating = () => UseMutationResult<unknown, unknown, RatingUpdateModel, unknown>;

const usePutUpgradeRating: UsePutUpgradeRating = () => {
  const { put } = useAxios();

  return useMutation((data) => extractData(put(`ratings`, data)));
};

export { usePutUpgradeRating };
export type { UsePutUpgradeRating };
