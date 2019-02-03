import swal from 'sweetalert2';
import { OperationResult } from '../../clientapi/client';

export class BaseComponent {
  attemptConnection<T>(promise: Promise<T>): Promise<T> {
    return new Promise((resolve, reject) => {
      promise
      .then(r => {
        resolve(r);
      }).catch(e => {
        this.showError(e.message);
        reject(e);
      });
  });
  }

  assertResult<T>(apiResult: any): Promise<T> {
      return new Promise((resolve, reject) => {
          if (!apiResult.isSuccess) {
            if (apiResult.message) {
              this.showError(apiResult.message);
            } else {
              console.error(apiResult);
              this.showError('Sorry, something went wrong');
            }

          } else {
            resolve(apiResult.data);
          }
      });
  }
  protected showError(title: string) {
    try {
      swal({
        type: 'error',
        title: title
      });
    } catch (e) {
      console.error(e);
    }
  }
}
