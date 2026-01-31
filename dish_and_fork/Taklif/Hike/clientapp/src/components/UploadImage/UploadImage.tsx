import { AddAPhoto } from '@mui/icons-material';
import { Box, CircularProgress } from '@mui/material';
import imageCompression from 'browser-image-compression';
import PromiseFileReader from 'promise-file-reader';
import { ChangeEvent, FC, useEffect, useState } from 'react';
import { v4 as uuidv4 } from 'uuid';

import { usePostFile } from '~/api';

import { COMPRESSION_OPTIONS, CropModal } from '../CropModal/CropModal';

type UploadImageProps = {
  accept?: string;
  disabled?: boolean;
  photo?: string;
  onComplete: (file: string[]) => void;
  height?: string;
  width?: string;
  borderRadius?: number | string;
  cropping?: boolean;
};

const UploadImage: FC<UploadImageProps> = ({
  accept = '',
  borderRadius = 2.5,
  disabled,
  height = '100px',
  onComplete,
  photo = '',
  width = '100px',
  cropping = true,
}) => {
  const { isLoading, mutateAsync } = usePostFile();
  const [open, setOpen] = useState(false);
  const [images, setImages] = useState<string[]>([]);

  const onUploaded = (event: ChangeEvent<HTMLInputElement>) => {
    const { files } = event.target;
    const imagesPromises = [];

    if (files) {
      for (let i = 0; i <= files.length; i += 1) {
        const file = event.target.files?.item(i);

        if (file) {
          imagesPromises.push(PromiseFileReader.readAsDataURL(file));
        }
      }

      Promise.all(imagesPromises).then((data) => {
        setImages(data);
      });
    }
  };

  const buildFormData = (file: Blob) => {
    const formData = new FormData();
    formData.append('file', file, `${uuidv4()}.jpg`);

    return formData;
  };

  useEffect(() => {
    if (images.length) {
      if (cropping) {
        setOpen(true);
      } else {
        const fetchPromises: Promise<unknown>[] = [];

        images.forEach((image) => {
          const p = fetch(image)
            .then((res) => res.blob())
            .then((blob) => {
              return imageCompression(
                new File([blob], 'blob.jpg', { type: 'image/jpg', lastModified: new Date().getTime() }),
                COMPRESSION_OPTIONS,
              );
            })
            .then((blob) => {
              const sizeInMB = (blob.size / (1024 * 1024)).toFixed(2);

              console.log(`${sizeInMB} Mb`);

              return mutateAsync(buildFormData(blob));
            });

          fetchPromises.push(p);
        });

        Promise.all(fetchPromises).then((data) => {
          onComplete(data as string[]);
        });
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [images]);

  const onCloseHandler = () => {
    setOpen(false);
  };

  const onOkHandler = async (file: Blob) => {
    mutateAsync(buildFormData(file)).then((data) => {
      setOpen(false);
      onComplete([data]);
    });
  };

  return (
    <>
      <Box
        bgcolor="common.grey"
        borderRadius={borderRadius}
        color="rgba(255, 255, 255, 0.87)"
        component="label"
        display="block"
        height={height}
        overflow="hidden"
        position="relative"
        sx={{
          '&::after': {
            content: '""',
            display: 'block',
            height: '100%',
          },

          cursor: 'pointer',
        }}
        width={width}
      >
        {cropping ? (
          <CropModal isLoading={isLoading} open={open} image={images[0]} onClose={onCloseHandler} onOk={onOkHandler} />
        ) : undefined}

        <Box
          alignItems="center"
          display="flex"
          height={1}
          justifyContent="center"
          left={0}
          position="absolute"
          top={0}
          width={1}
        >
          {isLoading ? (
            <CircularProgress color="inherit" size="large" />
          ) : (
            <>
              <Box component="img" marginX="auto" src={photo ?? ''} width={1} />
              <Box position="absolute">
                <AddAPhoto fontSize="large" />
              </Box>
            </>
          )}

          <Box
            accept={accept}
            component="input"
            disabled={disabled || isLoading}
            height={0}
            onChange={onUploaded}
            overflow="hidden"
            type="file"
            width={0}
            multiple={!cropping}
          />
        </Box>
      </Box>
    </>
  );
};

export { UploadImage };
export type { UploadImageProps };
