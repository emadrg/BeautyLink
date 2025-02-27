export default function Validator() {
  this.configuration = {};
  this.currentKey = [];
  this.checkOnlyOnSubmit = false;

  this.forProperty = (name, value = "") => {
    const keys = name.split(".");

    this.currentKey = keys;

    for (const keyIndex in keys) {
      this.configuration = {
        ...this.configuration,
        [keys[keyIndex]]:
          keyIndex == keys.length - 1
            ? {
                value,
                validations: [],
              }
            : {
                ...this.configuration[keys[keyIndex]],
              },
      };
    }

    return this;
  };

  this.check = (callbackFn, message) => {
    for (const keyIndex in this.currentKey) {
      this.configuration = {
        ...this.configuration,
        [this.currentKey[keyIndex]]:
          keyIndex == this.currentKey.length - 1
            ? {
                ...this.configuration[this.currentKey[keyIndex]],
                validations: [
                  ...this.configuration[this.currentKey[keyIndex]].validations,
                  {
                    check: callbackFn,
                    errorMessage: message,
                  },
                ],
              }
            : {
                ...this.configuration[this.currentKey[keyIndex]],
              },
      };
    }

    return this;
  };

  this.checkFields = (firstField, secondField, message) => {
    if (firstField !== secondField) {
      this.configuration = {
        ...this.configuration,
        ["confirmPassword"]: {
          ...this.configuration["confirmPassword"],
          validations: [
            ...this.configuration["confirmPassword"].validations,
            {
              errorMessage: message,
            },
          ],
        },
      };
    }
  };

  this.applyCheckOnlyOnSubmit = () => {
    this.checkOnlyOnSubmit = true;
    return this;
  };

  this.getConfiguration = () => {
    return this.configuration;
  };

  this.getCheckOnlyOnSubmit = () => {
    return this.checkOnlyOnSubmit;
  };
}
