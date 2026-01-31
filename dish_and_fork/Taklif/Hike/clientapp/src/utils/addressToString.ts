import { AddressReadModel } from '~/types';

type AddressToString = (address: AddressReadModel) => string;

const addressToString: AddressToString = ({
  apartmentNumber,
  city,
  country,
  entrance,
  house,
  intercom,
  region,
  street,
  zipCode,
}) =>
  [
    zipCode,
    country,
    city !== region && region,
    city,
    street,
    house && `дом ${house}`,
    entrance && `подъезд ${entrance}`,
    apartmentNumber && `oф./кв. ${apartmentNumber}`,
    intercom && `домофон - ${intercom}`,
  ]
    .filter((item) => !!item)
    .join(', ');

export { addressToString };
