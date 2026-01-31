import { PartnerUpdateModel, PeriodModel } from '~/types';

type FormData = Pick<
  PartnerUpdateModel,
  | 'id'
  | 'description'
  | 'imageId'
  | 'workingDays'
  | 'workingTime'
  | 'address'
  | 'contactEmail'
  | 'title'
  | 'contactPhone'
  | 'registrationAddress'
  | 'isPickupEnabled'
> &
  PeriodModel & {
    inn: string;
  };

export type { FormData };
