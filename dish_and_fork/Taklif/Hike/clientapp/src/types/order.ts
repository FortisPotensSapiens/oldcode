import { AddressReadModel, OrderCreateModel } from './swagger';

type LastOrderInfo = Partial<Omit<OrderCreateModel, 'recipientAddress'> & AddressReadModel>;
type CustomCreateOrderModel = Omit<OrderCreateModel, 'recipientAddress'> & {
  recipientAddress: Omit<AddressReadModel, 'id' | 'zipCode' | 'longitude' | 'latitude'>;
};
export type { CustomCreateOrderModel, LastOrderInfo };
