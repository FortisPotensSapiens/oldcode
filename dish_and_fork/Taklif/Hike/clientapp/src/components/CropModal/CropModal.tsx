import { Modal } from 'antd';
import imageCompression from 'browser-image-compression';
import { FC, useEffect, useRef, useState } from 'react';
import Cropper, { ReactCropperElement } from 'react-cropper';

import 'cropperjs/dist/cropper.css';

export const COMPRESSION_OPTIONS = {
  maxSizeMB: 0.5,
  maxWidthOrHeight: 500,
  maxIteration: 100,
  useWebWorker: true,
  alwaysKeepResolution: true,
};

const CropModal: FC<{
  open: boolean;
  image: string;
  onClose: () => void;
  onOk: (data: Blob) => void;
  isLoading?: boolean;
}> = ({ open, image, onClose, onOk, isLoading = false }) => {
  const [loading, setLoading] = useState(isLoading);
  const cropperRef = useRef<ReactCropperElement>(null);

  const onOkHandler = () => {
    const cropper = cropperRef.current?.cropper;

    if (cropper) {
      cropper
        .getCroppedCanvas({
          fillColor: '#fff',
        })
        .toBlob(
          async (data) => {
            if (data) {
              setLoading(true);

              const file = new File([data], 'blob.jpg', { type: 'image/jpg', lastModified: new Date().getTime() });

              const compressedFile = await imageCompression(file, COMPRESSION_OPTIONS);

              const sizeInMB = (compressedFile.size / (1024 * 1024)).toFixed(2);

              console.log(`${sizeInMB} Mb`);

              setLoading(false);
              onOk(compressedFile);
            }
          },
          'image/jpeg',
          0.8,
        );
    }
  };

  useEffect(() => {
    setLoading(isLoading);
  }, [isLoading]);

  const onLoaded = () => {
    console.log('onLoad');
    const cropper = cropperRef.current?.cropper as any;
    const containerData = cropper.getContainerData();
    cropper.setCropBoxData({ width: 400, height: 400, left: containerData.width / 2 - 200, top: 0 });
  };

  return (
    <Modal
      title="Подготовка изображения"
      open={open}
      onCancel={onClose}
      onOk={onOkHandler}
      okButtonProps={{
        loading,
      }}
      cancelButtonProps={{
        disabled: loading,
      }}
      cancelText="Отмена"
      okText="Готово"
      maskClosable={false}
    >
      <Cropper
        src={image}
        ready={onLoaded}
        style={{ height: 400, width: '100%' }}
        aspectRatio={1 / 1}
        ref={cropperRef}
      />
    </Modal>
  );
};

export { CropModal };
