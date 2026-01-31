export interface Row {
  buyerName: string;
  number: number;
  id: string;
  summ: number;
  createDate: string;
  status: string;
  isReadyToDelivery: boolean;
  sellerDeliveryTrackingUrl?: string;
}

export interface Column {
  after?: (item: Row) => unknown;
  format?: (item: Row) => unknown;
  id: 'number' | 'buyerName' | 'summ' | 'createDate' | 'status' | 'actions';
  label: string;
}
